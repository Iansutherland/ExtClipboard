## This solution contains an application that attempts to extend the Winodws Clipboard

-Captures clipboard items and saves them in Redis
-Allows user to paste anyting from the extended clipboard

## [Redis](https://redis.io/commands/)
I'm running Redis in a docker container
`
docker run --name some-redis -p 6379:6379 -d redis
`

And you can run commands with redis-cli to udpate redis with values to display
`
docker run --rm -it --name rediscli --net host redis redis-cli -h localhost -p 6379
`