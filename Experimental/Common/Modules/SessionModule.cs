using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using Experimental.Common.Builders;
using Experimental.Common.Objects;

namespace Experimental.Common.Modules
{

  [Group("session")]
  public class SessionModule : ModuleBase<SocketCommandContext>
  {
    [Command("start")]
    [RequireContext(ContextType.Guild)]
    public async Task Begin()
    {
      var store = Global.GetStore(Context.Guild);

      var session = store.CreateObject
        <GroupSession, GroupSessionBuilder>(
      x => x
         .WithChannel((ITextChannel)Context.Channel)
      );

      await session.BeginAsync();
    }

    [Command("end")]
    [RequireContext(ContextType.Guild)]
    public async Task End()
    {
      var store = Global.GetStore(Context.Guild);
      var sessions = store?.GetObjects<GroupSession>();
      var filter = sessions?.FirstOrDefault(x => x.IsStarted);

      if (filter == null)
      {
        return;
      }

      await filter.EndAsync();
    }
  }

}