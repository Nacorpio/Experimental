using System;
using System.Collections.Generic;
using System.Linq;

using Experimental.API.Collections;
using Experimental.Common.Templates;

using JetBrains.Annotations;

namespace Experimental.Common.Collections
{

  public class TemplateMap : ITemplateMap
  {
    private readonly Dictionary<Guid, TemplateBase> _templateGuidMap;

    public TemplateMap()
    {
      _templateGuidMap = new Dictionary<Guid, TemplateBase>();
    }

    public TemplateBase this[Guid templateGuid] => Get(templateGuid);

    public void Add([NotNull] TemplateBase t)
    {
      if (Contains(t))
      {
        return;
      }

      _templateGuidMap.Add(t.Guid, t);
    }

    public TTemplate New<TTemplate>(params object[] args)
      where TTemplate : TemplateBase
    {
      var template = (TTemplate)Activator.CreateInstance(typeof(TTemplate), args);

      if (Contains(template))
      {
        return default;
      }

      _templateGuidMap.Add(template.Guid, template);
      return template;
    }


    public bool Contains(TemplateBase template)
    {
      return Contains(template.Guid);
    }

    public bool Contains(Guid guid)
    {
      return _templateGuidMap.ContainsKey(guid);
    }

    public bool Contains([NotNull] string templateName)
    {
      return _templateGuidMap.Values.Any(
        x => string.Equals(
            x.Name,
            templateName,
            StringComparison.InvariantCultureIgnoreCase
          )
          || string.Equals(
            x.DisplayName,
            templateName,
            StringComparison.InvariantCultureIgnoreCase
          )
      );
    }

    public TTemplate Get<TTemplate>([NotNull] Guid templateGuid)
      where TTemplate : TemplateBase
    {
      return (TTemplate)(_templateGuidMap.TryGetValue(templateGuid, out var template) ? template : default);
    }

    public TemplateBase Get([NotNull] Guid templateGuid)
    {
      return _templateGuidMap.TryGetValue(templateGuid, out var template) ? template : default;
    }

    public TTemplate GetByName<TTemplate>([NotNull] string templateName)
      where TTemplate : TemplateBase
    {
      return Contains(templateName) ? (TTemplate)_templateGuidMap.Values.FirstOrDefault(
        x => string.Equals(
            x.Name,
            templateName,
            StringComparison.InvariantCultureIgnoreCase
          )
          || string.Equals(
            x.DisplayName,
            templateName,
            StringComparison.InvariantCultureIgnoreCase
          )
      ) : default;
    }
  }

}