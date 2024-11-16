CHANGES LOG, DATE: 11/16/2024
Scripts changes:
    - - - - - - - - - -
    AIDrive.cs
        Optimized component caching
        Implemented a health system that disable tank after getting shot 4 times.
    - - - - - - - - - -
    Drive.cs:
        Optimized component caching, might implement pooling for the explosion object later.
        Implemented a health system that disable tank after getting shot 4 times.
    - - - - - - - - - -
    ObjectPooler.cs:
        Moved pool initialization to the Awake call, also changed its implementation so the poolSize now depends on the number of users. Might implement explosion pooling later.
    - - - - - - - - - -
    Shell.cs:
        Optimized component caching.
    - - - - - - - - - -

