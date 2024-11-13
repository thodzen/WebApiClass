FROM postgres:16.2-bullseye

COPY ./db/software.sql /docker-entrypoint-initdb.d/software.sql

ENV POSTGRES_PASSWORD=password
ENV POSTGRES_USER=user
ENV PGDATA=/data