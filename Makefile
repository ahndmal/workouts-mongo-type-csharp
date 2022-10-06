deploy:
      gcloud functions deploy workouts-mongo-type-csharp --gen2 \
    		--runtime=dotnet3 \
    		--region=us-central1 \
    		--source=. \
    		--entry-point=HelloHttp.Function \
    		--trigger-http \
    		--allow-unauthenticated