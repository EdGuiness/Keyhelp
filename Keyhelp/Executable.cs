using System;
using System.IO;

namespace Keyhelp
{
    class Executable : IEquatable<Executable>
    {
        public string FullPath { get; }
        public string Filename => Path.GetFileName(FullPath);
        public string FilenameShort => Path.GetFileNameWithoutExtension(FullPath);

        public Executable(string fullPath)
        {
            FullPath = fullPath;
        }
        
        public bool Equals(Executable other)
        {
            if (other == null) return false;
            if (FullPath == null || other.FullPath == null) return false;
            return FullPath.Equals(other.FullPath, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Executable) obj);
        }

        public override int GetHashCode()
        {
            return (FullPath != null ? FullPath.GetHashCode() : 0);
        }
    }
}