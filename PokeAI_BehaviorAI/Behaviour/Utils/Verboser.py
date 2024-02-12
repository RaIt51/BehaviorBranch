from time import time
from Behaviour.Config import Config


class Verboser:
    def __init__(self, config: Config) -> None:
        self.config = config

    def print(self, text: str, level: int) -> None:
        if self.config.verbose >= level:
            print(text)

            if self.config.logging:
                with open(self.config.log_file, "a", encoding="utf-8") as f:
                    f.write(text + "\n")

    def start_timer(self) -> None:
        self.time_start = time()

    def finish_timer(self, label: str) -> None:
        text = f"[{label}] Time elapsed: {time() - self.time_start} seconds"
        print(text)

        if self.config.logging:
            with open(self.config.log_file, "a", encoding="utf-8") as f:
                f.write(text + "\n")
