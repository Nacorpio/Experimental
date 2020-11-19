using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Experimental.Common.Objects
{

  public class Community : ManagedObject
  {
    internal Community(Guid guid) : base(guid)
    {
      Sessions = new List<GroupSession>();
      Profiles = new List<Profile>();
    }

    public Community()
    {
      Sessions = null;
      Profiles = null;
    }

    public List<GroupSession> Sessions { get; internal set; }
    public List<Profile> Profiles { get; internal set; }
    public List <Group> Groups { get; internal set; }

    public override Task InitializeAsync()
    {
      return base.InitializeAsync();
    }
  }

}