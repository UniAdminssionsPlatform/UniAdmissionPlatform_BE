namespace UniAdmissionPlatform.WebApi
{
    public class Film
    {
        public string Name { get; set; }
        public string MainActor { get; set; }

        public Film(string name, string mainActor)
        {
            Name = name;
            MainActor = mainActor;
        }
    }
}