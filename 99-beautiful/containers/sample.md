## Starting Splunk Container for Dev

```shell
docker run -d -p 44401:8000 -p 44402:8088 --name splunk-dev -e SPLUNK_START_ARGS=--accept-license -e SPLUNK_PASSWORD=changeit -v {full path of project}/99-beautiful/containers/splunk:/tmp/defaults splunk/splunk
```