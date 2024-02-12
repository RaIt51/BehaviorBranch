from pathlib import Path
from Behaviour.Config import Config
from Behaviour.BehaviourController.BehaviourBranch import BehaviourBranch
from Behaviour.BehaviourController.BehaviourThinker.LLMFetcher import (
    LLMFetcher,
)
from Behaviour.BehaviourController.BehaviourThinker.BehaviourCodeReader import (
    read_behaviour_code,
)


class BehaviourThinker:
    """
    Make new behaviour branch
    """

    def __init__(self, config: Config) -> None:
        self.config = config

        self.code_prompt_template = self._read_code_prompt_template()

    def think(self, prompt_text: str) -> BehaviourBranch:
        # think
        prompt = self._make_prompt(prompt_text)

        # fetch from LLM
        tried = 0
        while True:
            fetcher = LLMFetcher(self.config)
            codes_action = fetcher.fetch_next_action(prompt)

            branch = None
            for code_action in codes_action:
                try:
                    branch = read_behaviour_code(code_action)

                    break
                except Exception as e:
                    print(e)
                    continue

            # if no valid action found...
            if (branch is None):
                if  (tried < 3):
                    # ...repeat
                    if self.config.verbose >= 1:
                        print("No valid action found.")
                        print("repeating...")
                    tried += 1
                    continue
                else:
                    # ...return None
                    return None
            # if valid action found...
            else:
                # ...return this branch
                return branch

    def _read_code_prompt_template(self) -> str:
        # template file is in the same directory as this .py file
        template_path = self.config.prompt_path
        with open(template_path, "r", encoding="utf8") as f:
            return f.read()

    def _make_prompt(self, prompt_text: str) -> str:
        return self.code_prompt_template.replace(
            self.config.prompt_alias, prompt_text
        )
