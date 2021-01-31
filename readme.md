1. u StartUp.cs 
            var cluster = Cluster.Builder()
            .AddContactPoints("192.168.99.100")
            .Build();
zameniti adresu sa adresom cassandra servera

2. u cassandri kreirati keyspace apcassandra

3. izvrsiti upite iz cassandra queries za za kreiranje tabela

4. pokrenuti .net core aplikaciju