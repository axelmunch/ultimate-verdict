-- init.sql

CREATE table if not EXISTS options (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE if not exists results (
    id SERIAL PRIMARY KEY,
    res TEXT CHECK (res IN ('winner', 'draw', 'inconclusive')) NOT NULL
);

CREATE TABLE if not exists decisions (
    id SERIAL PRIMARY KEY,
    score INTEGER NOT NULL
);

CREATE TABLE if not exists votes (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT,
    live_results BOOLEAN NOT NULL DEFAULT false,
    visibility TEXT CHECK (visibility IN ('public', 'private')) NOT NULL,
    type TEXT CHECK (type IN ('plural', 'ranked', 'weighted', 'elo')) NOT NULL,
    nb_rounds INTEGER NOT NULL,
    victory_condition TEXT CHECK (
        victory_condition IN ('none', 'majority', 'absolute majority', '2/3 majority', 'last man standing')
    ) NOT NULL,
    replay_on_draw BOOLEAN NOT NULL DEFAULT false,
    result_id INTEGER REFERENCES results(id)
);

CREATE TABLE if not exists rounds (
    id SERIAL PRIMARY KEY,
    vote_id INTEGER REFERENCES votes(id) ON DELETE CASCADE,
    name TEXT NOT NULL,
    start_time BIGINT NOT NULL,
    end_time BIGINT NOT NULL,
    result_id INTEGER REFERENCES results(id)
);
