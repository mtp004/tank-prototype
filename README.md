CHANGES LOG, DATE: 11/12/2024
Scripts changes:
    - - - - - - - - - -
    DestroyShell.cs:
        Deleted the C-sharp file because it is no longer used or serve any purpose.
    - - - - - - - - - -
    Drive.cs:
        Only changed Shoot() implementation to incorporate object pooling for the tank shells, calls GetObjectFromPool() to get a inactive shell and reset its properties such as angular velocity, rigidbody velocity, and transform rotation. Lastly set it active at tht barrel gameObject position and rotation.
    - - - - - - - - - -
    ObjectPooler.cs:
        Create a new script and attached to the GameManager gameObject. Contain a static instance of itself to allow other gameObject scripts to call the GetObjectFromPool() method to get an unactive gameObject from pool, assumes caller will activate the object as well as reset its attributes(IMPORTANT!, if not, the object will not function correctly).
        Contains the ReleaseObject(gameObject) method for the caller to deactivate the pooled gameObject.
        Contains CreatePool() method to initialize a pool with a predefined size.
    - - - - - - - - - -
    PooledObject.cs:
        Created the class to act as an attachable component to the pooled object to keep track of their index, this is a general purpose implementation because we can't always edit the script of a customed pooled object to add a variable to keep track of their pool index.
    - - - - - - - - - -
    Shell.cs:
        No longer add a predefined force to its rigid body in Start() to account for object pooling.
        In OnCollisionEnter(Collision collision) method, no longer destroy the shell but deactivate it instead.
    - - - - - - - - - -
README.md:
    Added a new Readme file to keep track of all the changes.
    Also added a TEMPLATE.md to have a predefined template for future md files and create a README file style from here on out.

