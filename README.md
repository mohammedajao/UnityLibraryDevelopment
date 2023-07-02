# About
Gummy is a database that offers event decoupling and a rudimentary CAD (Context-Aware Dialogue) system for Unity games.
This code takes heavy inspiration from Firewatch's system, Valve's GDC talk, and aarthificial's CAD system.

Rules are the core of Gummy. They determine the logical flow of events and fact mutations.
Events are simply ids that are mapped to invokable callbacks.
Facts are variables within our system. They're 32-bit integers mapped to some value within a blackboard.

Data Flow Breakdown:
- Blackboards are loaded from the save system
- Provider maps scopes assigned to it to the proper blackboards
- Events, rules, and facts are created
- Database initializes itself and binds a listener to pick the best rule for an event when selected
- Database uses the provider given to it to find the appropiate blackboard
- Rules are executed if they fulfill a criteria set and either update facts or trigger a chain of events
- A rules execution is an Enumerator and is overridable. Thus, onStart events are triggered immediately but onEnd events are triggered after execution
- Facts, rules, and events have their values shown within the current blackboard contexts the provider holds

# Design Decisions

**Why blackboards?**
As dictionaries grow, they become slower. Thus, we want to apply our entries (facts, rules, or events) to blackboards (dictionaries mapping entry ids to values) so we have a small dictionary/blackboard size and have quick hashtable operations. 

**How come the code doesn't work when I run the tool?**
My project is set up to work with the tool. Check your project. 

**Motivation**
I made this mainly to learn. I just saw the code already developed by other users and built the rest based on what was missing.

**Important Notes**
This is an educational project. It is not representative of clean code or how to properly code. It is not reflective of my coding pratices either. This is not production-ready tool but I hope it serves as motivation for others, helps as a basis, and more importantly, it was a fun explorative journey for me to learn how to build this. 

Unity's documentation is very lacking in guidance and the community isn't as educated on UI in its public spaces. Thus, a lot of my code came from looking at the source code for inspiration. And for my younger colleagues on my Github looking at this, remember it's important for things to work first. Clean up can happen later. Get it out there and working because nobody cares about how pretty it is on the inside.

# Next Steps
- Upload BlackboardModification definition
- Upload Fact Details View design
- Upload rules and facts and newer events being used
- Upload GameObjects and new tests that showcase the database working
- Address GetBlackboardForEntry() and hook database flow to App flow post-provider initialization
- Clean up UI code to use files and modularize UI code pieces 

# Resources
- https://www.youtube.com/watch?v=tAbBID3N64A
- https://www.youtube.com/watch?v=Y7-OoXqNYgY
- https://www.youtube.com/watch?v=wj-2vbiyHnI
- https://www.youtube.com/watch?v=1LlF5p5Od6A
- https://medium.com/wluper/how-do-dialogue-systems-decide-what-to-say-or-which-actions-to-take-b32ca223aff1

