# About Gummy
Gummy is a database that offers event decoupling and a rudimentary CAD (Context-Aware Dialogue) system for Unity games.
This code takes heavy inspiration from Firewatch's system, Valve's GDC talk, and aarthificial's CAD system.

Rules are the core of Gummy. They determine the logical flow of events and fact mutations.
Events are simply ids that are mapped to invokable callbacks.
Facts are variables within our system. They're 32-bit integers mapped to some value within a blackboard.

https://www.linkedin.com/feed/update/urn:li:activity:7104391180611637248?utm_source=share&utm_medium=member_desktop

Data Flow Breakdown:
- Blackboards are loaded from the save system
- Provider maps scopes assigned to it to the proper blackboards
- Events, rules, and facts are created
- Database initializes itself and binds a listener to pick the best rule for an event when selected
- Database uses the provider given to it to find the appropiate blackboard
- Rules are executed if they fulfill a criteria set and either update facts or trigger a chain of events
- A rules execution is an Enumerator and is overridable. Thus, onStart events are triggered immediately but onEnd events are triggered after execution
- Facts, rules, and events have their values shown within the current blackboard contexts the provider holds

# About TimeWizard

Gummy is the focus of this repo but TimeWizard is a chunk-based save system in development with a focus on a better and perhaps customizable UI flow. I don't think I'll get to it, but it's a common system I used to get an easy in-editor/in-game save editor.

# Design Decisions

**Why blackboards?**
As dictionaries grow, they become slower. Thus, we want to apply our entries (facts, rules, or events) to blackboards (dictionaries mapping entry ids to values) so we have a small dictionary/blackboard size and have quick hashtable operations. 


**Motivation**
I made this mainly to learn. I saw this already developed by other users and built the rest based on their talks.

# Important Notes

***Known Bug 1***
[Context 1](https://forum.unity.com/threads/argumentnullexception-value-cannot-be-null-parameter-name-_unity_self.1431901/)
[Context 2](https://forum.unity.com/threads/system-argumentnullexception-value-cannot-be-null-parameter-name-_unity_self.1447684/)
For usage, you cannot hit "Play" when the database (or some ScriptableObjects) is open for 2022 versions. Error is currently unknown due to Unity's UIToolkit behaviour. It is a fault of Unity's UIToolkit/UIElement system not being production-ready. It is an internal error with UIElements breaking due to the references and fields exposed due to Unity serialization when you hit play. Thus, if you open a ScriptableObject which Unity failed to build proper references for (it seems it's all as of testing), you will be met with infinite errors spamming your console.

In essence, if you are about to hit Play mode or exit it, make sure your inspector window is on a MonoBehaviour or Prefab and not a ScriptableObject.

***Known Bug 2***
Searching for entries doesn't work. This isn't a bug. I just didn't code it. I wanted to focus on other projects since the main features here are done.

Also, the dialogue bubble bug within this test scene in regards to two dialogue events running in parallel is not a bug. It's better for the developer to handle this case in my opinion. Perhaps you want it to run but have a more important dialogue running so you don't override or you do want to interrupt. That flow should be up to you.

# F.A.Q

*Adding a table scriptable object to the database in the inspector doesn't automatically run Setup.*

Right click the tables pane or an empty area in it and click refresh. The table list will update and you should get your table. I have to wait for Unity's SO issues with VisualElements to be resolved to address this.

Also, next time you hit play or scripts reload, the table will be set up. 

*Entry values don't reflect the current active context*
Current values of entries aren't under the umbrella of a single context. That's by design. Say entryA is under a temporary context (EmptyBlackboard) and entryB is under a persistent context served by the provider but also has a temporary contextual value, the database is designed to show the value of the most recently used context. It can't assume what the current context is because that'd require hooking onto the provider which should be a lightweight one-way interface.

Thus if ContextA is used after ContextZ for entryB, entryB's value in ContextA will be shown in the database.

*There's a lot of boiler plate code it seems with the speaker object runtime sets*
Yes. You can make your own wrapper to simplify the boilerplate based on your own provider. I'm showcasing the required objects at play here.

# About

This is an educational project. It is not representative of clean code or how to properly code. It is not reflective of my coding pratices either. This is not production-ready tool but I hope it serves as motivation for others, helps as a basis, and more importantly, it was a fun explorative journey for me to learn how to build this. 

Unity's documentation is very lacking in guidance and the community isn't as educated on UI in its public spaces. Thus, a lot of my code came from looking at the source code for inspiration. And for my younger colleagues on my Github looking at this, remember it's important for things to work first. Clean up can happen later. Get it out there and working because nobody cares about how pretty it is on the inside.

# Project Goals
The goal of these libraries/frameworks are to offer strong decoupling of code and a clean data flow. It also aims to speed up logical control. TimeWizard aims to decouple and ramp up saving by handling what save data is actively referenced in memory, offer non-technical access to manipulating these saves, flow hooks, and allow the game to get saves from different environments.

Gummy is aimed at decoupling logic control and asynchronous logical events without the need to code. Thus a writer can work on dialogue in Gummy or a designer can use it to describe new actions/behaviours for AI. It's applications are pretty large.

# Next Steps
- Upload GameObjects and new tests that showcase the database working
- Address GetBlackboardForEntry() and hook database flow to App flow post-provider initialization
- Clean up UI code to use files and modularize UI code pieces 

# Resources
- https://www.youtube.com/watch?v=tAbBID3N64A
- https://www.youtube.com/watch?v=Y7-OoXqNYgY
- https://www.youtube.com/watch?v=wj-2vbiyHnI
- https://www.youtube.com/watch?v=1LlF5p5Od6A
- https://medium.com/wluper/how-do-dialogue-systems-decide-what-to-say-or-which-actions-to-take-b32ca223aff1

