namespace Common
{
    [Serializable]
    public class User
    {
        public String Name { get; set; }
        public override bool Equals(object obj)
        {
            return obj is User other && other.Name == Name;
        }
    }
}