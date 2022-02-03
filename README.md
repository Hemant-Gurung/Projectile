# Projectile-Path
#DESCRIPTION

Projectile path is a motion that is experienced by a object that moves along a certain path under the effect of a gravity. This path in real life is also affected by air-resistance, but in this project the air resistance is neglected.
 
The Goal of this project is to give the projectile object a correct velocity, angle and force to be able to reach the user defined target in a specified time.

![PATHTracerGif](https://user-images.githubusercontent.com/84324141/148465933-abfe8640-5315-4f7f-9e87-23080b81916f.gif)

# IMPLEMENTATION

The project is implemented using unity engine.

# Maths required for the implementation.
 The project uses kinematic formulas.
- Displacement (S)
- Time interval (t)
- Initial velocity(Vi)
- Final Velocity(V)
- Acceleration due to gravity (g)(-9.8m/s^2)

Formulas used for projectile path.
- X(t) = Vxi*t+Xi
- Y(t)= Vyi*t-1/2(g)t^2+Yi

Angle Calculation
-> using cosine rule.

When the application starts, the required initial velocities are calculated by taking the target to reach in account. And according to that distance (initial projectile location and target to reach ), initial velocity and time, projectile path is calculated. Then angle that the shooter must be on is also calculated. 

#FINAL

![ezgif com-gif-maker_final](https://user-images.githubusercontent.com/84324141/151022106-aa8efbde-ba8d-470b-b7ef-df7692c9788c.gif)


#FUTURE WORK.

This project is just the tip of the projectile path prediction topic. A lot can be done following this process to exactly simulate the path that a projectile takes to reach a target by taking other forces such as friction, shape of a thrown object, air resistance etc. into account.
