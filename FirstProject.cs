using System;
using System.Collections.Generic;

namespace EntityRepositorySample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create two instances of EntityRepository
            var server = new EntityRepository();
            var client = new EntityRepository();

            // Create several entities and add them to the server's list
            server.AddEntity(new Entity { Name = "Entity 1", Position = new Vector3(1, 2, 3) });
            server.AddEntity(new Entity { Name = "Entity 2", Position = new Vector3(4, 5, 6) });
            server.AddEntity(new Entity { Name = "Entity 3", Position = new Vector3(7, 8, 9) });

            // Run a loop to update the client with the changes from the server
            while (true)
            {
                // Get a list of rows with updated information from the server
                var rows = server.Update();

                // Update the client with the changes from the server
                client.Update(rows);

                // Read input from the console
                Console.WriteLine("Enter entity number and command (e.g. 1 left):");
                var input = Console.ReadLine();

                // Parse the input
                var inputParts = input.Split(' ');
                var entityNumber = int.Parse(inputParts[0]);
                var command = inputParts[1];

                // Find the entity in the server and update its position
                var entity = server.GetEntity(entityNumber);
                if (entity != null)
                {
                    switch (command)
                    {
                        case "left":
                            entity.Position = new Vector3(entity.Position.X - 1, entity.Position.Y, entity.Position.Z);
                            break;
                        case "right":
                            entity.Position = new Vector3(entity.Position.X + 1, entity.Position.Y, entity.Position.Z);
                            break;
                        case "up":
                            entity.Position = new Vector3(entity.Position.X, entity.Position.Y + 1, entity.Position.Z);
                            break;
                        case "down":
                            entity.Position = new Vector3(entity.Position.X, entity.Position.Y - 1, entity.Position.Z);
                            break;
                    }
                }

                // Output the status of the server and client entities
                Console.WriteLine("Server entities:");
                foreach (var e in server.Entities)
                {
                    Console.WriteLine($"{e.Name}: {e.Position}");
                }
                Console.WriteLine("Client entities:");
                foreach (var e in client.Entities)
                {
                    Console.WriteLine($"{e.Name}: {e.Position}");
                }
            }
        }
    }

    class EntityRepository
    {
        public List<Entity> Entities { get; set; } = new List<Entity>();

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public Entity GetEntity(int entityNumber)
        {
            foreach (var entity in Entities)
                    {
            if (entity.Number == entityNumber)
            {
                return entity;
            }
        }
        return null;
    }

    public List<string> Update()
    {
        var rows = new List<string>();
        foreach (var entity in Entities)
        {
            if (entity.HasChanged)
            {
                rows.Add(entity.Serialize());
                entity.HasChanged = false;
            }
        }
        return rows;
    }

    public void Update(List<string> rows)
    {
        foreach (var row in rows)
        {
            var parts = row.Split(':');
            var entityNumber = int.Parse(parts[0]);
            var entity = GetEntity(entityNumber);
            if (entity == null)
            {
                entity = new Entity();
                Entities.Add(entity);
            }
            entity.Deserialize(row);
        }
    }
}

class Entity
{
    public Vector3 Position { get; set; }
    public string Name { get; set; }
    public bool HasChanged { get; set; }
    public int Number { get; set; }

    public string Serialize()
    {
        return $"{Number}:{Name}:{Position.X}:{Position.Y}:{Position.Z}";
    }

    public void Deserialize(string row)
    {
        var parts = row.Split(':');
        Number = int.Parse(parts[0]);
        Name = parts[1];
        Position = new Vector3(
            int.Parse(parts[2]),
            int.Parse(parts[3]),
            int.Parse(parts[4])
        );
    }
}

struct Vector3
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Vector3(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }
}

}
           
