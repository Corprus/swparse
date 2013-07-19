namespace SWParse.LogStructure
{
    public struct LogSkill
    {
        public string Name;
        public int? Id;
        public LogSkill(string logString)
        {
            Name = string.Empty;
            Id = null;
            if (logString != string.Empty)
            {
                var parsedName = LogParser.ParseString(@"^(?<name>.*?) \{(?<id1>\d*?)\}$", logString);
                Name = parsedName[0];
                int id;
                if (int.TryParse(parsedName[1], out id))
                {
                    Id = id;
                }
            }
        }
    }
}
