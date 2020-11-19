using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Experimental.API;
using Experimental.Common.Builders;
using Experimental.Common.Objects;
using Experimental.Common.Storage;

using JetBrains.Annotations;

namespace Experimental
{

  public static partial class Global
  {
    /// <summary>
    /// Gets the global Discord client used on all guilds.
    /// </summary>
    public static readonly DiscordSocketClient Client;

    /// <summary>
    /// Gets the global storage container map for separate guilds.
    /// </summary>
    public static readonly Dictionary<Guid, Store> StoreGuidMap;

    /// <summary>
    /// Gets the community storage container map.
    /// </summary>
    public static readonly Dictionary<Guid, Community> CommunityGuidMap;

    /// <summary>
    /// Gets the guild stores mapped with their unique identifiers.
    /// </summary>
    public static readonly Dictionary<long, Store> StoreGuildIdMap;

    /// <summary>
    /// Gets the global storage container.
    /// </summary>
    public static readonly Store Store;

    /// <summary>
    /// Gets the guilds currently connected.
    /// </summary>
    public static readonly Dictionary<ulong, Community> CommunityIdMap;


    /// <summary>
    /// Initializes a new static instance of the <see cref="Global"/> class.
    /// </summary>
    static Global()
    {
      Client = new DiscordSocketClient();

      CommunityGuidMap = new Dictionary<Guid, Community>();
      CommunityIdMap = new Dictionary<ulong, Community>();

      StoreGuildIdMap = new Dictionary<long, Store>();
      StoreGuidMap = new Dictionary<Guid, Store>();

      Store = new Store();

      Client.GuildAvailable += OnGuildAvailable;
      Client.MessageUpdated += OnMessageUpdated;
      Client.MessageDeleted += OnMessageDeleted;
    }

    internal static async Task PrepareStoresAsync()
    {
      var a = Assembly.GetCallingAssembly();
      var t = a.GetTypes();

      var filtered = t.Where(
        x => x.GetInterfaces()
         .Any(
            type =>
              type == typeof(ManagedObject) &&
              type.IsClass &&
              !type.IsAbstract
          )
      );

      var types = filtered as Type[] ?? filtered.ToArray();

      foreach (var guild in Client.Guilds)
      {
        var community = GetCommunity(guild);

        foreach (var type in types)
        {
          var store = community.Store;

          StoreGuildIdMap.Add((long)guild.Id, (Store)store);
          StoreGuidMap.Add(community.Guid, (Store)store);

          store.GetManager(type);
        }
      }
    }

    /// <summary>
    /// Returns the storage container for the specified community.
    /// </summary>
    /// <param name="community">The community to get the store for.</param>
    /// <returns>A reference to the storage container, if found; otherwise, <c>null</c>.</returns>
    public static Store GetStore([NotNull] Community community)
    {
      if (!StoreGuidMap.TryGetValue(community.Guid, out var store))
      {
        store = new Store(community, (SocketGuild)community.Guild);
        StoreGuildIdMap.Add((long)community.Guild.Id, store);
      }

      return store;
    }

    /// <summary>
    /// Returns the storage container for the specified guild.
    /// </summary>
    /// <param name="guild">The guild to get the store for.</param>
    /// <returns>A reference to the storage container, if found; otherwise, <c>null</c>.</returns>
    public static Store GetStore([NotNull] IGuild guild)
    {
      if (!CommunityIdMap.TryGetValue((ulong)guild.Id, out var community))
      {
        return null;
      }

      var guildId = (long)guild.Id;

      if (!StoreGuildIdMap.TryGetValue(guildId, out var store))
      {
        store = new Store(community, (SocketGuild)guild);
        StoreGuildIdMap.Add(guildId, store);
      }

      return store;
    }

    /// <summary>
    /// Finds and returns a substance with the specified identifier.
    /// </summary>
    /// <param name="substanceGuid">The GUID of the substance to find.</param>
    /// <returns>A reference to the substance, if found; otherwise, <c>null</c>.</returns>
    public static Substance GetSubstance(Guid substanceGuid)
    {
      return Store.GetObject<Substance>(substanceGuid);
    }

    /// <summary>
    /// Finds and returns a substance matching the specified name.
    /// </summary>
    /// <param name="substanceName">The name of the substance to find.</param>
    /// <returns>A reference to the substance, if found; otherwise, <c>null</c>.</returns>
    public static Substance GetSubstance([NotNull] string substanceName)
    {
      return Store.GetObjects<Substance>()
       .FirstOrDefault(x => x.Name.Equals(substanceName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns the community associated with the specified guild.
    /// </summary>
    /// <param name="guild">The guild to find the community for.</param>
    /// <returns>A reference to the community, if found; otherwise, <c>null</c>.</returns>
    public static Community GetCommunity([NotNull] IGuild guild)
      => GetCommunity(guild.Id);

    /// <summary>
    /// Finds and returns a community associated with a guild with the specified GUID.
    /// </summary>
    /// <param name="guildId">The GUID of the guild to find the associated community of.</param>
    /// <returns>A reference to the community, if found; otherwise, <c>null</c>.</returns>
    public static Community GetCommunity
      (ulong guildId) =>
      (Community)(CommunityIdMap.TryGetValue(
        guildId,
        out var community
      )
        ? community
        : ManagedObject.Null);

    private static IEnumerable<Type> _GetManagedObjectTypes()
    {
      var a = Assembly.GetCallingAssembly();
      var t = a.GetTypes();

      var filtered = t.Where(
        x => x.GetInterfaces()
         .Any(
            type =>
              type == typeof(ManagedObject) &&
              type.IsClass &&
              !type.IsAbstract
          )
      );

      return filtered as Type[] ?? filtered.ToArray();
    }

    private static async Task OnGuildAvailable(SocketGuild guild)
    {
      var community = Store
       .CreateObject<Community, CommunityBuilder>(
          x => x.WithGuild(guild)
        );

      var types = _GetManagedObjectTypes();

      foreach (var type in types)
      {
        community.Store.GetManager(type);
      }

      community.Guild = guild;

      CommunityGuidMap.Add(community.Guid, community);
      StoreGuidMap.Add(community.Guid, new Store(community));
      CommunityIdMap.Add(guild.Id, community);
    }

    private static async Task OnMessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage arg2, ISocketMessageChannel arg3)
    {
    }

    private static async Task OnMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
    {
    }
  }

}