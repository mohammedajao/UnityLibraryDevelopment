# About
Gummy is a database that offers event decoupling and a rudimentary CAD (Context-Aware Dialogue) system for Unity games.
This code takes heavy inspiration from Firewatch, Valve's GDC talk, and aarthificial's CAD system.

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

**The code does not work when I use it**
It's not supposed to. This repo isn't a tool. It's just a past development project. If I upload the completed version at some point, feel free to use it. The point is the idea. While I do have a working version, I also have a set of serialized data and my project is set up properly for this code. This repo is not a guide or official library/framework but the data flow explains how it works and you can use the logic to make your own.

**Motivation**
I made this mainly to learn. I just saw the code already developed by other users and built the rest based on what was missing.

# Next Steps
- Upload BlackboardModification definition
- Upload Fact Details View design
- Upload rules and facts and newer events being used
- Upload GameObjects and new tests that showcase the database working
- Address GetBlackboardForEntry() and hook database flow to App flow post-provider initialization

# Resources
- https://www.youtube.com/watch?v=tAbBID3N64A
- https://www.youtube.com/watch?v=Y7-OoXqNYgY
- https://www.youtube.com/watch?v=wj-2vbiyHnI
- https://www.youtube.com/watch?v=1LlF5p5Od6A
- https://medium.com/wluper/how-do-dialogue-systems-decide-what-to-say-or-which-actions-to-take-b32ca223aff1

