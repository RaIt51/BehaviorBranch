from Behaviour.Config import Config


def clean(config: Config) -> None:
    _erace_cache(config)


def _erace_cache(config: Config) -> None:
    # Erace audio directory
    for file in config.audio_directory.glob("*.wav"):
        file.unlink()


# instant run
if __name__ == "__main__":
    config = Config()
    clean(config)
