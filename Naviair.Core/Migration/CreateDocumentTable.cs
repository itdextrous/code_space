using Microsoft.Extensions.Logging;
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
    public class BlogCommentsComposer : ComponentComposer<naviairDocumentComponent>
    {
    }
    public class naviairDocumentComponent : IComponent
    {
        private readonly ICoreScopeProvider _coreScopeProvider;
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public naviairDocumentComponent(
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
            var migrationPlan = new MigrationPlan("naviairDocument");

            // This is the steps we need to take
            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty)
                .To<AddDocumentTable>("naviairdocument-db");

            // Go and upgrade our site (Will check if it needs to do the work or not)
            // Based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_migrationPlanExecutor, _coreScopeProvider, _keyValueService);
        }

        public void Terminate()
        {
        }
    }
    public class AddDocumentTable : MigrationBase
    {
        public AddDocumentTable(IMigrationContext context) : base(context)
        {
        }
        protected override void Migrate()
        {
            Logger.LogDebug("Running migration {MigrationStep}", "AddCommentsTable");

            // Lots of methods available in the MigrationBase class - discover with this.
            if (TableExists("naviairDocument") == false)
            {
                Create.Table<Document>().Do();
            }
            else
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", "naviairDocument");
            }
        }
        [TableName("naviairDocument")]
        [PrimaryKey("id", AutoIncrement = true)]
        public class Document
        {
            [Column("id")]
            [JsonProperty(PropertyName = "id")]
            [PrimaryKeyColumn(AutoIncrement = true)]
            public int Id { get; set; }

            [Column("nodeId")]
            [JsonProperty(PropertyName = "parentId")]
            public int NodeId { get; set; }

            [Column("publishAt")]
            [JsonProperty(PropertyName = "publishAt")]
            [NullSetting(NullSetting = NullSettings.Null)]
            public DateTime? PublishAt { get; set; }

            [Column("href")]
            [JsonProperty(PropertyName = "href")]
            [Length(4000)]
            public string Href { get; set; }

            [Column("published")]
            [JsonProperty(PropertyName = "published")]
            public bool Published { get; set; }

            [Ignore]
            [JsonProperty(PropertyName = "link")]
            public string Link { get; set; }
        }
    }
}