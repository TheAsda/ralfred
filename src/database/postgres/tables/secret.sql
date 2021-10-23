CREATE TABLE public.secret (
	id      VARCHAR(36)  NOT NULL PRIMARY KEY,
	groupId VARCHAR(36)  NOT NULL,
	name    VARCHAR(255) NOT NULL,
	value   TEXT         NOT NULL,
	isFile  BOOLEAN      NOT NULL
);