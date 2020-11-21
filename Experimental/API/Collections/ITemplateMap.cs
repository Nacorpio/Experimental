using System;

using Experimental.Common.Templates;

using JetBrains.Annotations;

namespace Experimental.API.Collections
{

  public interface ITemplateMap
  {
    TemplateBase this[Guid templateGuid] { get; }

    TTemplate New<TTemplate>(params object[] args) where TTemplate : TemplateBase;

    TTemplate Get<TTemplate>([NotNull] Guid templateGuid) where TTemplate : TemplateBase;
    TemplateBase Get([NotNull] Guid templateGuid);

    TTemplate GetByName<TTemplate>([NotNull] string templateName) where TTemplate : TemplateBase;

    bool Contains(string templateName);
  }

}