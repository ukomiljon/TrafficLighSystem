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


