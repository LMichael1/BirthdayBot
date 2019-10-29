FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR BirthdayBot/app

# Copy csproj and restore as distinct layers
COPY BirthdayBot.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR BirthdayBot/app
COPY --from=build-env BirthdayBot/app/out ./
CMD dotnet BirthdayBot.dll