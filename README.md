# TrafficLighSystem

It uses this sequence of traffic lights. https://www.youtube.com/watch?v=eZ33_lEjgxo

For example:
South/North: G, Y, R, R, R, R
West/East:   R, R, R, G, Y, R


The first 2 sequences might be 1 second different, if the unit test fails it needs to run again.  

# Brief documentation:
 
1. TrafficLightServer project - it makes communication with client, it runs with SignalR which can send message to client
2. TrafficLightService project- core architecture domain knowledge here.
3. Client Console project  - to see results
4. Common project - it provides DTO, JSON serializer, interfaces for settings and timer where may use across projects
5. XUniteTestTrafficLightSystem - Unit test for various scenarios with normal / pickhours


# Definition:

It is required to implement a traffic light system with 4 sets of lights, as follows. <br /><br />
Lights 1: Traffic is travelling south <br />
Lights 2: Traffic is travelling west <br />
Lights 3: Traffic is travelling north<br />
Lights 4: Traffic is travelling east<br /><br />
The lights in which traffic is travelling on the same axis can be green at the same time. During normal hours all lights stay green for 20 seconds, but during peak times north and south lights are green for 40 seconds while west and east are green for 10 seconds. Peak hours are 0800 to 1000 and 1700 to 1900. Yellow lights are shown for 5 seconds before red lights are shown. Red lights stay on until the cross-traffic is red for at least 4 seconds, once a red light goes off then the green is shown for the required time(eg the sequence is reset). <br />

Advanced: At this intersection north bound traffic has a green right-turn signal, which stops the south bound traffic and allows north bound traffic to turn right. This is green at the end of north/south green light and stays green for 10 seconds. During this time north bound is green, north right-turn is green and all other lights are red. 

# Implementation/Outcomes:

1.	Implement a front-end and backend (you can use ‘dotnet new’ templates of your choice)
2.	The backend will contain the logic and state of the running traffic lights. The front-end will be a visual representation of the traffic lights, with the data served from the backend. 
3.	There’s no need to have a perfect design on the front end, something simple and functional is fine (unless this is an area of strength you would like to show off). Noting* we will review the client side code.
4.	There’s no need to implement entity framework (or similar) to store the data in a database, a in-memory store is fine
5.	Code needs to follow architecture & best practices for enterprise grade systems
