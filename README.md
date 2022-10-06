
Deploy

```shell
gcloud functions deploy workouts-mongo-type-csharp \
--gen2 \
--runtime=dotnet3 \
--region=us-central1 \
--source=. \
--entry-point=HelloHttp.Function \
--trigger-http \
--allow-unauthenticated
```
### LOGS

```
gcloud functions logs read
gcloud functions logs read FUNCTION_NAME --execution-id EXECUTION_ID
```
## Mongo 
### Install Mongo Tools

```
mongodb+srv://@cluster0.t1yi6.mongodb.net/myFirstDatabase?appName=mongosh+1.6.0
```