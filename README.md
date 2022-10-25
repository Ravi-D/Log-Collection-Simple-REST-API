# REST API Log Collection 📄

This Web API solution written in C# / .NET 6.0.7 allows a user to retrieve lines of data from a given log file in a directory. The results can be optionally filtered with keywords as well as restricted to a maximum number of lines requested.

- The testing of this project was aided with local test files and hardcoded directories.
- I didn't write a programmatic way for a user to edit this without digging into the source code, so a small console interface or utilizing unique appsettings.json files would save users some setup time.
- To run this solution on your machine, you will have to modify the code directly in order to set the correct filepath containing your logs.

## 💻 Execution Instructions  ##

    1. Unzip the application file.
	2. Open the `LogCollection` solution (.sln) in Visual Studio and navigate to the `Constants.cs` file.
	3. Update the `PROD_SOURCE_DIRECTORY`, set this variable to a directory containing log files you'd like to test the API with.

#### Via Visual Studio ####
    1. Press CTRL+F5 to begin the solution without debugging, or F5 for Debugger.
    2. 🎉 Your solution should be running! 💨
    3. This method is considered Debugging/Development mode, so your browser will open up the Swagger URL: `https://localhost:####/swagger/index.html`
    3. Consult the next section for instructions on how to use Swagger or Postman to interact with the API.

#### Via Executable File ####
    1. Navigate to your LogCollection application file (\...\LogCollection\LogCollection\bin\Release\net6.0\) and double click it.
    2. The API should be running! 🏃🏽‍♂️
    3. View your Command Prompt window and note down the localhost URL that is being used.
    4. Use the first line of information -> `Now listening on https://localhost:7182`
    5. You will need this URL for the Postman instructions below - we are not using the Swagger UI because this is the RELEASE/"Production" version of the application. If you notice in the `Program.cs` file, this is why the Swagger isn't generated.

#### Via Command Line ####
    1. Navigate to your LogCollection application file (\...\LogCollection\LogCollection\bin\Release\net6.0\)
    2. Execute the following command `dotnet run .\LogCollection.exe --project "\...\LogCollection\LogCollection\LogCollection.csproj"`
        2.5. You may optionally add `--urls=https://localhost:4DigitsHere` to use your own port number.
    4. View your Command Prompt window and note down the localhost URL that is being used.
    5. Use the first line of information -> `Now listening on https://localhost:7182`
    6. You will need this URL for the Postman instructions below - we are not using the Swagger UI because this is the RELEASE/"Production" version of the application. If you notice in the `Program.cs` file, this is why the Swagger isn't generated.

📝 Note: 

- You'll notice in the FileRetrievalController GetLogByName method that the file name you provide the API will be appended to `PROD_SOURCE_DIRECTORY`, so ensure this is correct!
			A new `LogRequest` object will be created with the parameters you provided, and the file handler will get to work!
## 😎 Swagger / Postman 📮 ## 
	
Example `curl` commands for your reference:

Log: `curl -X 'GET' \
  'https://localhost:7182/FileRetrieval/api/get-log-by-name?fileName=install.log&linesToReturn=15' \
  -H 'accept: */*'`

Log with lines: `curl -X 'GET' \
  'https://localhost:7182/FileRetrieval/api/get-log-by-name?fileName=install.log&linesToReturn=15' \
  -H 'accept: */*'`

Log with filter: `curl -X 'GET' \
  'https://localhost:7182/FileRetrieval/api/get-log-by-name?fileName=install.log&searchTerm=error' \
  -H 'accept: */*'`


Log with lines and filter: `curl -X 'GET' \
  'https://localhost:7182/FileRetrieval/api/get-log-by-name?fileName=install.log&linesToReturn=15&searchTerm=error' \
  -H 'accept: */*'`



Swagger
1. Using the browser window that Visual Studio opened for you, click on the API you'd like to test. 
2. Enter your parameters and observe the results!


Postman
1. Using Postman or another REST client, send GET requests per the specifications outlined in Swagger. 

    1.5. If you are running the application via executable, check the first line of your command prompt window. It will tell you which localhost route to use on Postman.

2. Your Postman should look something like this:
GET `https://localhost:####/FileRetrieval/api/get-log-by-name?fileName=log-small.txt&linesToReturn=5&searchTerm=2022-10-15`

	- Parameters:
	    - A file name (string) WITH file extension is required.
    	- A number (integer) representing how many lines you would like returned from a given log is NOT required, and any funny business integer values you can think of sticking in here handled accordingly. Or at least they should be...
    	- A keyword (string) identifying a filter to be applied to log results. Only lines containing this keyword will be returned, but it's also bound by the optional max lines parameter #2.

#### 🏗 NuGet Packages  ####
- Microsoft.Extensions.DependencyInjection v6.0.1
- Microsoft.NET.Test.Sdk v17.3.2
- Moq v4.18.2
- Swashbuckle.AspNetCore v6.2.3
- NUnit v3.13.3
- NUnitTest3Adapter v4.2.1

#### 🧪 Testing  ####
This project utilizes the NUnit testing framework. The test log is saved with a company name in the full path and I didn't feel like changing it, so I have redacted the string. Tests in their current state will not pass 100%.

- If you would like to run tests and are curious about NUnit tips and tricks, read on and don't miss out on the What's Next section below!

Because this is an API project, in order to run NUnit tests or manual Swagger/Postman testing, your Visual Studio must be currently running the project with your browser displaying the Swagger UI.

This project relies on local files to verify application behavior, and the test suites are very bare bones at the moment. They are provided to give you an idea of some basic NUnit functionality interacting with the code rather than ensure a production-ready solution.

 
1. Right click on the LogCollection solution and click Run Tests. You can also enter the Tests files and run individual tests manually.
	1.1. Or set breakpoints manually and Debug Tests.

#### 🧠 Some thoughts I had....  ####
##### Initial Planning
- Which file reading library would work best for this use case? (Keeping in mind performance, and ease of use.)
	- MemoryMappedFiles versus StreamReader. At first I decided to push
- The ever-present "how should I structure all of this, which File I/O library do I use "internal monologue that possibly results in a tiny mid-development refactor.
    -  Update at the end of the project: Check out the What's Next section for details 🙃
- Developing on a Windows machine for a solution specifically targeting Unix machines.
	- Have to keep in mind the differences with line ending characters "\\r and \\n" on Windows, versus just "\\n" on Unix machines.


##### 🤔 What's Next?  ####
- Continue to build using Objected Oriented Principles: Maintain a StreamReaderFileHandler class as well as MemoryMappedFileHandler class, both implementing IFileHandler.
	- The current solution is the result of beginning to address the prompt and structure OOP around a MemoryMappedFileHandler with the Mapped File and Mapped View Stream being passed in via Dependency Injection, but then having to refactor due to the difficulty I had satisfying the assessment constraints around optional parameters.
    - MemoryMapped file handling is extremely performant for random access and reading subsections of a large file, byte by byte. It's good stuff, but not when you try to force it to read entire multi GB files in this fashion with only one "subsection!"
    - StreamReader on the other hand, is able to sequentially read lines one at a time, which makes applying logic around max lines and keyword filtering wayyy easier. 
   - You don't have to keep track of a huge number of bytes and piece them together after the fact. Verifying if you've returned the exact amount of lines requested, with the lines correctly checked for filters leaves too many edge case scenarios. 
     - Use the best tool for the job with the time constraints you have. ⚒ 
- Additional logging, but that's a given. Currently I am logging a basic trace containing the calling function, the file handler's current correlation ID for tracking purposes, and the thrown exception causing the logging in the first place.
- The lines / filter conditional logic in the code is not as clean as I would like.

- Testing
	- The end result of unit testing that lines/filter logic would be a very clean testing matrix nonetheless. You would be able to supply many different inputs into just one function rather than creating unique methods for each combination of parameters.
	- FYI on NUnit `[TestCase(1, 2, 3)]` attributes -> https://docs.nunit.org/articles/nunit/writing-tests/attributes/testcase.html
	- You can also create your own helper `IEnumerable<TestCaseData> LogRequestsWithFilters` object for example. You will need to `get{ yield return new TestCaseData(x,y,z)}` inside it as many times as you like. Each `TestCaseData` object should contain parameters that make sense with the IEnumerable name you made.
	- Your test decorator will just be `[TestCaseSource(nameof(LogRequestsWithFilters))]` instead of a long stacked list of inputs, or even worse, a ton of new test methods.

Time to "log" off! ✍

-Ravi Dhebar
