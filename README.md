# BS05RequestHelper

BS05RequestHelper is a .NET library designed to simplify sending API requests and handling responses with ease. It is specifically built to streamline HTTP requests and eliminate repetitive code.

Note: Currently, it only supports RestSharp, but HttpClient support will be added in future updates.

## Features
- Simple and intuitive API.
- Supports GET, POST HTTP requests.
- Automatic serialization/deserialization of JSON format.
- Automatic error handling after HTTP requests.
- Ability to send asynchronous requests.
- Upcoming: HttpClient and PUT,DELETE support.

## Installation

You can add the package to your project via NuGet:

```bash
dotnet add package BS05RequestHelper
```

## Usage

```c#
using BSRequestHelper.Concrete;

var helper = new BSRestSharpRequestHelper();
var requestModel = new MyRequestModel();
var response = await helper.PostAsync<MyResponseModel>("my-url", requestModel);

```

## Adding Headers

```c#
var headers = new Dictionary<string, string>
    {
        { "header-1", "foo"},
        { "header-2", "bar"}
    };

var response = await helper.PostAsync<MyResponseModel>("my-url", requestModel,headers);

```

## Using with dependency injection
add this to your startup or program.cs

```c#
services.AddScoped<IBSRestSharpRequestHelper, BSRestSharpRequestHelper>();
```

```c#
public class MyClass{
  readonly IBSRestSharpRequestHelper _requestHelper;
  public MyClass(IBSRestSharpRequestHelper requestHelper){ 
     _requestHelper = requestHelper;
  }
}

var response = await _requestHelper.PostAsync<MyResponseModel>("my-url", requestModel);


```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
