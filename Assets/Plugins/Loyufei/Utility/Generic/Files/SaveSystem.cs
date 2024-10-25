using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei
{
    [Serializable]
    public abstract class SaveSystem<TSaveable> : ISaveSystem<TSaveable> where TSaveable : ISaveable
    {
        [SerializeField]
        protected EUnitySavePath _UnitySavePath;
        [SerializeField]
        protected string         _SavePath;
        [SerializeField]
        protected string         _FileName;

        public string SavePath
        {
            get
            {
                var unitySavePath = string.Empty;

                switch (_UnitySavePath)
                {
                    case EUnitySavePath.PersistanceDataPath:
                        unitySavePath = Application.persistentDataPath;
                        break;
                    case EUnitySavePath.DataPath:
                        unitySavePath = Application.dataPath;
                        break;
                    default:
                        unitySavePath = string.Empty;
                        break;
                }

                return Path.Combine(unitySavePath, _SavePath);
            }
        }

        public string FileName => _FileName + ".json";

        public virtual TSaveable Saveable { get; protected set; }

        public virtual TSaveable FetchData() 
        {
            Saveable = this.Load<TSaveable>() ?? Activator.CreateInstance<TSaveable>();

            return Saveable;
        }
    }
}
