# SE2-CBT

## backend

[![](https://github.com/shilangyu/SE2-CBT/workflows/backend-ci/badge.svg)](https://github.com/shilangyu/SE2-CBT/actions)

Stack:

- [ASP.NET Core](https://github.com/dotnet/aspnetcore)
- [Entity Framework Core](https://github.com/dotnet/efcore)

### run locally

```sh
cd backend
docker build -t se2-back . # build image
docker run -it -p 3000:80 se2-back # create container
```

### development

You will only need [.NET 6](https://dotnet.microsoft.com/en-us/download). Then cd into `backend/` and run the project with `dotnet run`. In vscode you can install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) and run the project by pressing F5.

## frontend

[![](https://github.com/shilangyu/SE2-CBT/workflows/web-ci/badge.svg)](https://github.com/shilangyu/SE2-CBT/actions)

Stack:

- [React](https://reactjs.org)
- [MUI](https://mui.com)
- [Zustand](https://zustand-demo.pmnd.rs)
- [Emotion](https://emotion.sh)

### run locally

```sh
cd web
docker build -t se2-front . # build image
docker run -it -p 3000:80 se2-front # create container
```

### development

You will only need [node.js 17](https://nodejs.org/en/download/current/). Then cd into `web/` and run the project with `npm run dev`.
