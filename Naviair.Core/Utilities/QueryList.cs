using Microsoft.Extensions.Configuration;
using NaviAir.Core.Config;
using System.Text;

namespace NaviAir.Core.Utilities
{
 
    public class QueryList
    {
      
        public readonly string PublishRule =
            //top level rule
            " and (" +
                //this body OR the next
                "(" +
                    //documents which should be and are published
                    "(GETUTCDATE() > dbo.naviairDocument.publishAt and dbo.naviairDocument.published = 1 ) " +
                        "and " +
                    //node which doesn't have an unpublish date OR should not be unpublished yet
                    "(dbo.naviairNode.unpublishAt is null or GETUTCDATE() < dbo.naviairNode.unpublishAt)" +
                ") " +
            "OR " +
                //the previous body OR this one
                "(" +
                    //It is a dir and it is published (according to boolean variable only)
                    "isDir = 1 AND dbo.naviairNode.published = 1" +
                ")" +
            ")"; //end top level rule

        public readonly string AllUnpublishedFilesQuery = @"SELECT
                                                      naviairNode.id,
                                                      naviairNode.parentId,
                                                      naviairNode.isDir,
                                                      case when naviairNode.isDir = 1 then naviairNode.published else naviairDocument.published end as published
                                                    FROM dbo.naviairNode
                                                    LEFT JOIN naviairDocument
                                                      ON naviairDocument.nodeId = naviairNode.Id
                                                    WHERE 
                                                      (naviairNode.unpublishAt IS NULL OR GETUTCDATE() < naviairNode.unpublishAt)
                                                        AND 
                                                      (naviairDocument.published = 0 OR naviairNode.isDir = 1)";

        public readonly StringBuilder PublicTreeQuery = new StringBuilder($@"SELECT
                                      dbo.naviairNode.id,
                                      dbo.naviairNode.parentId,
                                      dbo.naviairNode.name,
                                      dbo.naviairNode.title,
                                      dbo.naviairNode.tags,
                                      dbo.naviairNode.isDir,
                                      dbo.naviairNode.unpublishAt,
                                      dbo.naviairDocument.publishAt,
                                      (CASE
                                        WHEN isDir = 0 THEN '{AppSettings.StorageUrl}{AppSettings.StorageContainer}' + dbo.naviairDocument.href
                                        ELSE dbo.naviairDocument.href
                                      END) AS href,
                                      dbo.naviairDocument.id AS naviairDocumentid,
                                      (SELECT
                                        (CASE
                                          WHEN EXISTS (SELECT
                                              NULL AS [EMPTY]
                                            FROM dbo.naviairNode AS n
											LEFT JOIN dbo.naviairDocument AS d ON d.nodeId = n.id
                                            WHERE n.parentId = dbo.naviairNode.Id AND (d.publishAt < GETUTCDATE() or n.publishAt < GETUTCDATE() or (n.publishAt is null and n.isDir = 1))) THEN 1
                                          ELSE 0
                                        END))
                                      AS hasChildren
                                    FROM dbo.naviairDocument
                                    RIGHT OUTER JOIN dbo.naviairNode
                                      ON dbo.naviairDocument.nodeId = dbo.naviairNode.id");

        public readonly StringBuilder UnpublishedFilesSubListQuery = new StringBuilder(@";WITH unpublishedFiles (id, parentId, name, title, unpublishAt, path, isDir, level)
                                                            AS (SELECT
                                                              naviairNode.id,
                                                              naviairNode.parentId,
                                                              naviairNode.name,
                                                              naviairNode.title,
                                                              naviairNode.unpublishAt,
                                                              'Home' + naviairNode.path,
                                                              naviairNode.isDir,
                                                              0 AS level
                                                            FROM dbo.naviairNode
                                                            WHERE {0}

                                                            UNION ALL

                                                            SELECT
                                                              naviairNode.id,
                                                              naviairNode.parentId,
                                                              naviairNode.name,
                                                              naviairNode.title,
                                                              naviairNode.unpublishAt,
                                                              'Home' + naviairNode.path + '/',
                                                              naviairNode.isDir,
                                                              level + 1
                                                            FROM dbo.naviairNode
                                                            INNER JOIN unpublishedFiles AS d
                                                              ON naviairNode.parentId = d.id)
                                                            SELECT
                                                              unpublishedFiles.id,
                                                              unpublishedFiles.parentId,
                                                              unpublishedFiles.name,
                                                              unpublishedFiles.title,
                                                              naviairDocument.publishAt,
                                                              unpublishedFiles.unpublishAt,
                                                              naviairDocument.href,
                                                              unpublishedFiles.path,
                                                              '{1}{2}' + naviairDocument.href AS link,
                                                              unpublishedFiles.isDir
                                                            FROM unpublishedFiles
                                                            LEFT JOIN naviairDocument
                                                              ON naviairDocument.nodeId = unpublishedFiles.Id
                                                            WHERE (GETUTCDATE() < naviairDocument.publishAt OR 
                                                                naviairDocument.publishAt IS NULL)
                                                            AND (unpublishedFiles.unpublishAt IS NULL
                                                            OR GETUTCDATE() < unpublishedFiles.unpublishAt)
                                                            AND unpublishedFiles.isDir = 0");


        public readonly StringBuilder UnpublishedFilesQuery = new StringBuilder($@"SELECT
                                    naviairNode.id,
                                    naviairNode.parentId,
                                    naviairNode.name,
                                    naviairNode.title,
                                    naviairDocument.publishAt AS publishDocumentAt,
                                    naviairNode.publishAt AS publishNodeAt,
                                    naviairNode.unpublishAt,
                                    naviairDocument.href,
                                    naviairNode.path,
                                    '{AppSettings.StorageUrl}{AppSettings.StorageContainer}' + naviairDocument.href AS link,
                                    naviairNode.isDir,
                                    (SELECT
                                    (CASE
                                        WHEN EXISTS (SELECT
                                            NULL AS [EMPTY]
                                        FROM dbo.naviairNode AS n
                                        LEFT JOIN dbo.naviairDocument AS d
                                            ON d.nodeId = n.id
                                        WHERE n.parentId = dbo.naviairNode.Id
                                        AND (d.published = 0
                                        OR n.isDir = 1)) THEN 1
                                        ELSE 0
                                    END))
                                    AS hasChildren,
                                    naviairDocument.published
                                    FROM dbo.naviairNode
                                    LEFT JOIN naviairDocument
                                    ON naviairDocument.nodeId = naviairNode.Id
                                    LEFT JOIN (SELECT
                                    nodeId,
                                    MIN(publishAt) AS minPublishAt
                                    FROM naviairDocument
                                    WHERE published = 0
                                    GROUP BY nodeId) AS minPublishAts
                                    ON minPublishAts.nodeId = dbo.naviairNode.id");

        public readonly StringBuilder FilesAndFolderQuery = new StringBuilder($@"SELECT
                                naviairNode.id,
                                naviairNode.parentId,
                                naviairNode.[name],
                                naviairNode.title,
                                naviairDocument.publishAt,
                                naviairDocument.publishAt AS publishDocumentAt,
                                naviairNode.publishAt AS publishNodeAt,
                                naviairNode.unpublishAt,
                                naviairDocument.href,
                                naviairNode.path,
                                '{AppSettings.StorageUrl}{AppSettings.StorageContainer}' + naviairDocument.href AS link,
                                naviairNode.isDir,
                                naviairNode.published,
                                (SELECT
                                (CASE
                                    WHEN EXISTS (SELECT
                                        NULL AS [EMPTY]
                                    FROM dbo.naviairNode AS n
                                    LEFT JOIN dbo.naviairDocument AS d
                                        ON d.nodeId = n.id
                                    WHERE n.parentId = dbo.naviairNode.Id
                                    AND (d.published = 0
                                    OR n.isDir = 1)) THEN CAST(1 as bit)
                                    ELSE CAST(0 as bit)
                                END))
                                AS hasChildren,
                                CASE
                                WHEN (GETUTCDATE() > naviairDocument.publishAt AND
                                    (GETUTCDATE() < naviairNode.unpublishAt OR
                                    naviairNode.unpublishAt IS NULL)) THEN CAST(1 as bit)
                                WHEN naviairNode.isDir = 1 THEN NULL
                                ELSE CAST(0 as bit)
                                END AS published
                            FROM dbo.naviairNode
                            LEFT JOIN (SELECT
                                nodeId,
                                MIN(publishAt) AS minPublishAt
                            FROM naviairDocument
                            GROUP BY nodeId) AS minPublishAts
                                ON minPublishAts.nodeId = dbo.naviairNode.id
                            LEFT JOIN naviairDocument
                                ON naviairDocument.nodeId = naviairNode.Id
	                            WHERE (naviairDocument.publishAt = minPublishAts.minPublishAt OR naviairNode.isDir = 1)");
    }
}