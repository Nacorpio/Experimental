using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using UnitsNet;

namespace Experimental.Common.Objects.Data
{

  public class SubstanceUserDoseInfo : ManagedObject
  {
    internal SubstanceUserDoseInfo(Guid guid) : base(guid)
    {
    }

    public SubstanceUserDoseInfo()
    {
      UserId = null;
      SubstanceGuid = Guid.Empty;
      TakenAt = null;
      Amount = null;
    }


    [JsonProperty("user_id")]
    public ulong? UserId { get; internal set; }

    [JsonProperty("substance_guid")]
    public Guid SubstanceGuid { get; internal set; }


    [JsonProperty("taken_at")]
    public DateTimeOffset? TakenAt { get; internal set; }

    [JsonProperty("amount")]
    public Mass? Amount { get; internal set; }


    public override JObject ToJson()
    {
      var _ = base.ToJson();

      _["user_id"] = UserId;
      _["substance_guid"] = SubstanceGuid;
      _["taken_at"] = TakenAt;
      _["amount"] = Amount.ToString();

      return _;
    }

    public override void FromJson(JObject json)
    {
      base.FromJson(json);

      UserId = json.Value<ulong?>("user_id");
      SubstanceGuid = json.Value<Guid>("substance_guid");
      TakenAt = json.Value<DateTimeOffset?>("taken_at");
      Amount = Mass.Parse(json.Value<string>("amount"));
    }
  }

}