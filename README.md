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

# Next Steps


# Resources
- https://www.youtube.com/watch?v=tAbBID3N64A
- https://www.youtube.com/watch?v=Y7-OoXqNYgY
- https://www.youtube.com/watch?v=wj-2vbiyHnI
- https://www.youtube.com/watch?v=1LlF5p5Od6A
- https://medium.com/wluper/how-do-dialogue-systems-decide-what-to-say-or-which-actions-to-take-b32ca223aff1

