using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using NSwag.CodeGeneration.TypeScript;

var geenSwaggerUrl = "http://localhost:7000/swagger/v1/swagger.json";

var settings = new TypeScriptClientGeneratorSettings();

settings.GenerateDtoTypes = true;
settings.GenerateClientClasses = true;
settings.GenerateClientInterfaces = false;
settings.GenerateOptionalParameters = false;
settings.ExceptionClass = "ApiException";
settings.OperationNameGenerator = new MultipleClientsFromFirstTagAndOperationIdGenerator();
settings.InjectionTokenType = InjectionTokenType.InjectionToken;
settings.RxJsVersion = 7.0m;
settings.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Class;
settings.TypeScriptGeneratorSettings.TypeScriptVersion = 3.5M;
settings.HttpClass = HttpClass.HttpClient;
settings.Template = TypeScriptTemplate.Angular;
settings.ClassName = "{controller}Client";
settings.UseSingletonProvider = true;

settings.TypeScriptGeneratorSettings.DateTimeType = TypeScriptDateTimeType.Date;

var generator = new TypeScriptClientGenerator(await OpenApiDocument.FromUrlAsync(geenSwaggerUrl), settings);
var code = generator.GenerateFile();

var outFile = "../../../../../../Frontend/src/client/apiClient.ts";

if (File.Exists(outFile)) File.Delete(outFile);

Directory.CreateDirectory(Path.GetDirectoryName(outFile));

await File.WriteAllTextAsync(outFile, code);

Console.WriteLine(Directory.GetCurrentDirectory());

Console.WriteLine($"Writed: {outFile}");