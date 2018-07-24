# openIMIS Web Application

The openIMIS Web Application is the main component of the openIMIS infrastructure.
It is used to manage all openIMIS entities from any browser.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

In order to use and develop the openIMIS Web Application on your local machine, you first need to install:

* SQL Server instance (follow [database installation](https://github.com/openimis/database_ms_sqlserver))


### Installing

To make a copy of this project on your local machine, please clone the repository.

```
git clone https://github.com/openimis/web_app_vb
```

Restore the NuGet packages needed by the application using VS or [nuget CLI](https://www.nuget.org/downloads).

```
nuget restore
```

Before running the application, you need to change the connection string to connect to the 
database in the [Web.config](./IMIS/Web.config) file within [IMIS](./IMIS/) folder.

```
<add name="CHF_CENTRALConnectionString" connectionString="Data Source=[DatabaseIPAdress];Initial Catalog=IMIS;User ID=[ImisUserId];Password=[ImisUserPassword]" providerName="System.Data.SqlClient"/>
```


<!--## Running the tests

Explain how to run the automated tests for this system

### Break down into end to end tests

Explain what these tests test and why

```
Give an example
```

### And coding style tests

Explain what these tests test and why

```
Give an example
```-->

## Deployment

For deployment please read the [installation manual](http://openimis.readthedocs.io/en/latest/web_application_installation.html).

<!--## Built With

* [Visual Studio](https://visualstudio.microsoft.com/) - The web framework used
* [Dropwizard](http://www.dropwizard.io/1.0.2/docs/) - The web framework used
* [Maven](https://maven.apache.org/) - Dependency Management
* [ROME](https://rometools.github.io/rome/) - Used to generate RSS Feeds
-->

<!--## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.
-->

<!--## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags). 
-->

<!--## Authors

* **Billie Thompson** - *Initial work* - [PurpleBooth](https://github.com/PurpleBooth)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.
-->

## License

This project is licensed under the GNU AGPL v3 License - see the [LICENSE.md](LICENSE.md) file for details.

<!--## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
-->
