namespace LinqToObjects
{
    public class Person
    {
        public string Address { get; set; }
        public int Age { get; set; }
        public int Wieght { get; set; }

        public override string ToString()
        {
            return $"Age = {Age}, Address = {Address}, Wieght = {Wieght}";
        }
    }
}