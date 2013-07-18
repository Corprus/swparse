namespace SWParse.LogStructure
{
    public struct LogSkill
    {
        public string Name;
        public long? Id;
        public LogSkill(string logString)
        {
            Name = string.Empty;
            Id = null;
            if (logString != string.Empty)
            {
                var parsedName = LogParser.ParseString(@"^(?<name>.*?) \{(?<id1>\d*?)\}$", logString);
                Name = parsedName[0];
                long id;
                if (long.TryParse(parsedName[1], out id))
                {
                    Id = id;
                }
            }
        }
    }
}
