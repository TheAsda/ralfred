CREATE TABLE public.account (
	id                    UUID  NOT NULL PRIMARY KEY,
	name									VARCHAR(255) NULL,
	tokenHash             VARCHAR(128)  NULL,
	certificateThumbprint VARCHAR(40)  NULL
);

CREATE TABLE public.group (
	id   UUID   NOT NULL PRIMARY KEY,
	name VARCHAR(255)  NOT NULL,
	PATH VARCHAR(512)  NOT NULL
);

CREATE TABLE public.role (
	id UUID NOT NULL PRIMARY KEY
);

CREATE TABLE public.secret (
	id      UUID  NOT NULL PRIMARY KEY,
	groupId UUID  NOT NULL,
	name    VARCHAR(255) NOT NULL,
	value   TEXT         NOT NULL,
	isFile  BOOLEAN      NOT NULL
);