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
docker run --rm -p 3000:80 se2-back # create container
```

### development

You will only need [.NET 6](https://dotnet.microsoft.com/en-us/download). Then cd into `backend/` and run the project with `dotnet run`. In vscode you can install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) and run the project by pressing F5.

### integration tests

Run all with `dotnet test`. By default tests are performed in memory, however if `TEST_URL` environment variable is provided, tests are performed against that url.

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
docker run --rm -p 3001:80 se2-front # create container
```

### development

You will only need [node.js 17](https://nodejs.org/en/download/current/). Then cd into `web/` and run the project with `npm run dev`.

## integration-tests

Stack:

- [Python 3.10](https://www.python.org)
- [Pipenv](https://pipenv-es.readthedocs.io/es/stable/)
- [Google Chrome](https://www.google.com/chrome/) _- Chromium won't work_

### run locally

```sh
# set TEST_URL environment variable, e.g.: (.env file is supported)
export TEST_URL=http://127.0.0.1:3001

# optionally make the browser visible
export SELENIUM_SHOW=1

cd integration-tests
pipenv install
pipenv run python main.py
```
