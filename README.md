# EFCore Experiments
Entity framework core extensions to maybe use in other projects

To run migrations:

    dotnet ef database update --project EFCoreExperiments.DataContext --startup-project EFCoreExperiments.TestAPI

# Filtering, Sorting and pagination
For an easy way to generate queries for filtering, sorting and pagination for EF Core.
The projected is located under [Libs/EFCoreFiltering](https://github.com/Hinz3/EFCoreExperiments/tree/main/EFCoreFiltering)

## Configuration
In AppSettings its possible to set the default and max page size.
|Name| Default  |
|--|--|
| DefaultPageCount | 20 |
| MaxPageSize | 1000 |

## Usage
To use it first you have to dependency inject it into your IServiceCollection
```C#
services.UseFiltering(configuration);
```

In your repository you will have to inject `IRequestQuery` which will get the query parameters.

### Get paged response
On your `IQueryable<TEntity>` you will have the `ToListPagedAsync` which returns PageResponse with your entity type.
```C#
public async Task<PagedResponse<User>> GetUsers() => 
    await context.Users.AsNoTracking().ToListPagedAsync(request);
```
Example code is located [here](https://github.com/Hinz3/EFCoreExperiments/blob/75470ae07465f9c05171686e935ebba2fd80886b/EFCoreExperiments.Core/Repositories/UserRepository.cs#L16)

### API Query
These are the query parameters `IRequestQuery` uses.
|Name| Example |
|--|--|
| Filter | `[{"Property":"Id","Operator":"Eq","Value":"10"}]` |
| Sort | `[{"Property":"Id","IsAscending":false}]` |
| pageCount | 1 |
| pageSize | 10 |

Example request: 
``` HTTP
GET /api/Users?filter=[{"Property":"Id","Operator":"Eq","Value":"10"}]&sort=[{"Property":"Id","IsAscending":false}]&pageCount=1&pageSize=1000 HTTP/1.1
Host: localhost:7288
```
