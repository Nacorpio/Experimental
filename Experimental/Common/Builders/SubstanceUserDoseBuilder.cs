using System;

using Discord;

using Experimental.API;
using Experimental.Common.Objects;
using Experimental.Common.Objects.Data;

using JetBrains.Annotations;

using UnitsNet;

namespace Experimental.Common.Builders
{

  public class SubstanceUserDoseBuilder : BuilderBase<SubstanceUserDoseInfo>
  {
    public SubstanceUserDoseBuilder()
    {
      User = null;
      Substance = null;
      TakenAt = null;
      Amount = null;
    }

    public IUser User { get; set; }
    public Substance Substance { get; set; }

    public DateTimeOffset? TakenAt { get; set; }
    public Mass? Amount { get; set; }

    public SubstanceUserDoseBuilder WithUser([NotNull] IUser value)
    {
      User = value;
      return this;
    }

    public SubstanceUserDoseBuilder WithSubstance([NotNull] Substance value)
    {
      Substance = value;
      return this;
    }

    public SubstanceUserDoseBuilder WithTakenAt(DateTimeOffset value)
    {
      TakenAt = value;
      return this;
    }

    public SubstanceUserDoseBuilder WithAmount(Mass value)
    {
      Amount = value;
      return this;
    }

    public override SubstanceUserDoseInfo Build()
    {
      return new SubstanceUserDoseInfo(Guid.NewGuid())
      {
        UserId = User.Id,
        SubstanceGuid = Substance.Guid,
        TakenAt = TakenAt,
        Amount = Amount
      };
    }
  }

}