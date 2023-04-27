using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TimeWizard;

namespace TimeWizard.Persistence
{
    public class FileSaveLoader : ISaveLoader
    {
        private static readonly SaveContainer[] emptySaves = new SaveContainer[0];
        private readonly string workingDirectoryPath;

        public SaveContainer[] ListSaves()
        {
            EnsureWorkingDirectory();
            var directories = Directory.GetDirectories(workingDirectoryPath, "*", SearchOption.TopDirectoryOnly);
            if(directories.Length > 0)
            {
                return directories.Select(dir => new SaveContainer() {
                    Name = Path.GetFileName(dir),
                    CreatedAt = Directory.GetCreationTimeUtc(dir),
                    UpdatedAt = File.GetLastWriteTimeUtc(dir)
                }).ToArray();
            }
            return emptySaves;
        }

        public bool TrySave(string name, Chunk[] chunks, out Exception ex)
        {
            EnsureWorkingDirectory();
            ex = null;
            bool wasTempSave = false;
            try
            {
                var saveFolderPath = Path.Combine(workingDirectoryPath, name);
                if(Directory.Exists(saveFolderPath))
                {
                    saveFolderPath = Path.Combine(workingDirectoryPath, $"{name}_tmp");
                    wasTempSave = true;
                }
                Directory.CreateDirectory(saveFolderPath);
                foreach (var saveChunk in chunks)
                {
                    string scopedPath = Path.Combine(saveFolderPath, saveChunk.Name);
                    if(!Directory.Exists(scopedPath))
                    {
                        Directory.CreateDirectory(scopedPath);
                    }
                    string filePath = Path.Combine(scopedPath, $"{saveChunk.ID.ToString()}.json");
                    string json = JsonUtility.ToJson(saveChunk);
                    File.WriteAllText(filePath, json);
                }

                if(wasTempSave)
                {
                    var prevSave = Path.Combine(workingDirectoryPath, name);
                    Directory.Delete(prevSave, true);
                    Directory.Move(saveFolderPath, prevSave);
                }
                return true;
            } catch(Exception e)
            {
                ex = e;
                return false;
            }
        }

        public bool TryLoad(string name, out Chunk[] chunks, out Exception ex)
        {
            EnsureWorkingDirectory();
            ex = null;
            chunks = new Chunk[0];
            if(name == "")
            {
                ex = new Exception("[TimeWizard] Received an empty string for the SaveContainer.");
                return false;
            }
            try
            {
                string savePath = Path.Combine(workingDirectoryPath, name);
                var readChunks = new List<Chunk>();
                string[] dirs = Directory.GetDirectories(savePath, "*", System.IO.SearchOption.TopDirectoryOnly);
                foreach(string dir in dirs)
                {
                    string chunkName = Path.GetDirectoryName(dir);
                    string saveChunksPath = Path.Combine(savePath, dir);
                    var dataFiles = Directory.GetFiles(saveChunksPath);

                    foreach(var file in dataFiles)
                    {
                        Chunk saveChunk = JsonUtility.FromJson<Chunk>(File.ReadAllText(file));
                        // saveChunk.Name = Path.GetFileName(file);
                        readChunks.Add(saveChunk);
                    }
                }
                chunks = readChunks.ToArray();
                return true;
            } catch (Exception e) {
                ex = e;
                return false;
            }
        }

        public bool TryClearSave(string name, out Exception ex)
        {
            EnsureWorkingDirectory();
            ex = null;
            try
            {
                var path = Path.Combine(workingDirectoryPath, name);
                if(Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }
                return true;
            } catch (Exception e) {
                ex = e;
                return false;
            }
        }

        private void EnsureWorkingDirectory()
        {
            if(!Directory.Exists(workingDirectoryPath))
            {
                Directory.CreateDirectory(workingDirectoryPath);
            }
        }

        internal void Cleanup() // Currently throws exception if file explorer is viewing the folders
        {
            if(Directory.Exists(workingDirectoryPath))
            {
                Directory.Delete(workingDirectoryPath, true);
            }
        }

        public FileSaveLoader(string wdPath)
        {
            workingDirectoryPath = Path.Combine(Application.persistentDataPath, wdPath);
        }
    }
}