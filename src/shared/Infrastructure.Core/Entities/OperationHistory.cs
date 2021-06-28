﻿using Infrastructure.Core.Persistence;

namespace Infrastructure.Core.Entities
{
    public class OperationHistory : CoreEntity
    {
        public string RealmName { get; set; }
        public int RealmFaction { get; set; }
        public long Duration { get; set; }
        public int Inserted { get; set; }
        public int Deleted { get; set; }
        public int Updated { get; set; }
    }
}
