using System;
using System.Collections.Generic;
using System.Linq;

using Discord;

using Experimental.Common.Builders;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Experimental.Common.Objects
{

  public class Profile : ManagedObject
  {
    public new static Profile Null => new Profile();

    public static Profile Create
      ([NotNull] IGuild guild, [NotNull] Action<ProfileBuilder> _)
      => Global.GetStore(guild).CreateObject<Profile>(_);

    public static Profile Create
      ([NotNull] IGuild guild, [NotNull] IUser user) =>
      Create(guild, x => x.WithUser(user));


    internal Profile(Guid guid) : base(guid)
    {
      Doses = new List<SubstanceUserDoseInfo>();
    }

    public Profile()
    {
      UserId = null;
      Doses = null;
    }


    [JsonProperty("user_id")]
    public ulong? UserId { get; internal set; }

    [JsonProperty("doses")]
    public List<SubstanceUserDoseInfo> Doses { get; internal set; }


    public override JObject ToJson()
    {
      var _ = base.ToJson();

      _["user_id"] = UserId;
      _["doses"] = new JArray(Doses.Select(x => x.ToJson()));

      return _;
    }

    public override void FromJson(JObject json)
    {
      base.FromJson(json);

      UserId = json.Value<ulong?>("user_id");
      Doses = json["doses"]?.Select(
        x =>
        {
          var _ = Global.Store.CreateObject<SubstanceUserDoseInfo>();
          _.FromJson(x as JObject);

          return _;
        }
      ).ToList();
    }
  }

}