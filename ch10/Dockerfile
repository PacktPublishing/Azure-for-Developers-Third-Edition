FROM ubuntu:latest

RUN mkdir /job
WORKDIR /job

# Create a new file with a unique name each 30 seconds
CMD while true; do date +%s > $(date +%s).txt; sleep 30; done
