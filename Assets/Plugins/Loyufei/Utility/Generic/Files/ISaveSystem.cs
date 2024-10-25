using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loyufei
{
    public interface ISaveSystem
    {
        public string    SavePath { get; }
        public string    FileName { get; }
        public ISaveable Saveable { get; }

        public ISaveable FetchData();
    }

    public interface ISaveSystem<TSaveable> : ISaveSystem where TSaveable : ISaveable 
    {
        public new TSaveable Saveable { get; }

        public new TSaveable FetchData();

        ISaveable ISaveSystem.Saveable    => Saveable;
        ISaveable ISaveSystem.FetchData() => FetchData();
    }
}
