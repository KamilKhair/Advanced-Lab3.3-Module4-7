namespace LinqToObjects
{
    public class Worker
    {
        public int Age { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"ID = {Id}, Age = {Age}, Role = {Role}, Address = {Address}";
        }
    }
}