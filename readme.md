#k8-deploy steps:
dotnet build Project Azure Func .net7
-----------------------------------
#docker:
docker build -t mansoorkhalid2020/dataimport-azurefunctions .
docker push mansoorkhalid2020/dataimport-azurefunctions
-----------------------------------
#k8-Keda:

https://keda.sh/docs/2.0/concepts/authentication/
https://keda.sh/docs/2.0/authentication-providers/secret/
kubectl apply -f 1-k8-secrets.yml
kubectl apply -f 2-k8-kedascaler-triggerauth.yml
-----------------------------------
#regular k8 objects
kubectl apply -f 3-k8-deploy.yaml
kubectl apply -f 4-k8-service.yml
-----------------------------------
https://keda.sh/docs/2.0/scalers/azure-storage-queue/
https://keda.sh/docs/2.0/concepts/scaling-deployments/
kubectl apply -f 5-k8-kedascaler-scaledobject.yml

#clean up
kubectl delete deploy dataimport-azure-functions-deployment
kubectl delete ScaledObject dataimport-azure-functions-scale-object
kubectl delete Secret dataimport-azure-functions-secret