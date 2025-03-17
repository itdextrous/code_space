using Microsoft.Extensions.Logging;
using NaviAir.Core.Service;
using Newtonsoft.Json;
using NPoco;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;


namespace Naviair.Core.Migration
{
    public class naviairNodeComposer : ComponentComposer<naviairNodeComponent>
    {
    }
    public class naviairNodeComponent : IComponent
    {
        private readonly ICoreScopeProvider _coreScopeProvider;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public naviairNodeComponent(
            ICoreScopeProvider coreScopeProvider,
            IMigrationPlanExecutor migrationPlanExecutor,
            IKeyValueService keyValueService,
            IRuntimeState runtimeState)
        {
            _coreScopeProvider = coreScopeProvider;
            _migrationPlanExecutor = migrationPlanExecutor;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }

        public void Initialize()
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            // Create a migration plan for a specific project/feature
            // We can then track that latest migration state/step for this project/feature
            var migrationPlan = new MigrationPlan("naviairNode");

            // This is the steps we need to take
            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty)
                .To<AddNodeTable>("naviairnode-db");

            // Go and upgrade our site (Will check if it needs to do the work or not)
            // Based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
        }

        public void Terminate()
        {
        }
    }
    public class AddNodeTable : MigrationBase
    {
        public AddNodeTable(IMigrationContext context) : base(context)
        {
        }
        protected override void Migrate()
        {
            Logger.LogDebug("Running migration {MigrationStep}", "AddNodeTable");

            // Lots of methods available in the MigrationBase class - discover with this.
            if (TableExists("naviairNode") == false)
            {
                Create.Table<Node>().Do();
            }
            else
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", "naviairNode");
            }
        }
        [TableName("naviairNode")]
        [PrimaryKey("id", AutoIncrement = true)]
        public class Node
        {
            [Column("id")]
            [JsonProperty(PropertyName = "id")]
            [PrimaryKeyColumn(AutoIncrement = true)]
            public int Id { get; set; }

            [Column("parentId")]
            [NullSetting(NullSetting = NullSettings.Null)]
            [JsonProperty(PropertyName = "parentId")]
            public int? ParentId { get; set; }

            [Column("name")]
            [JsonProperty(PropertyName = "name")]
            [Length(500)]
            public string Name { get; set; }

            [Column("title")]
            [JsonProperty(PropertyName = "title")]
            [Length(500)]
            public string Title { get; set; }

            [Column("tags")]
            [JsonProperty(PropertyName = "tags")]
            [Length(500)]
            [NullSetting(NullSetting = NullSettings.Null)]
            public string Tags { get; set; }

            [Column("isDir")]
            [JsonProperty(PropertyName = "isDir")]
            public bool IsDir { get; set; }

            [Column("unpublishAt")]
            [JsonProperty(PropertyName = "unpublishAt")]
            [NullSetting(NullSetting = NullSettings.Null)]
            public DateTime? UnpublishAt { get; set; }

            [Column("published")]
            [NullSetting(NullSetting = NullSettings.Null)]
            [Obsolete("We want to eliminate this field from DB in the future")]
            public bool PublishedDb { get; set; }


            //[JsonProperty(PropertyName = "published")]
            //[Ignore]
            //public bool Published => PublishService.Create().ShouldBePublished(this);

            [Column("publishAt")]
            [JsonProperty(PropertyName = "publishAt")]
            [NullSetting(NullSetting = NullSettings.Null)]
            public DateTime? PublishAt { get; set; }

            [Column("path")]
            [JsonProperty(PropertyName = "path")]
            [Length(4000)]
            public string Path { get; set; }
        }
    }
}