import React, { ChangeEvent, FC, FormEvent, useState } from "react";
import axios from "axios";

const Images: FC = () => {
  const [image, setImage] = useState<File>(null);

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const options = [
      e.currentTarget.option1.value,
      e.currentTarget.option2.value,
    ];

    const formData = new FormData();
    formData.append("image", image);
    formData.append("question", e.currentTarget.question.value);
    formData.append("options", JSON.stringify(options));
    formData.append("correctAnswer", e.currentTarget.correctAnswer.value);
    formData.append("hint", e.currentTarget.hint.value);
    formData.append("imageFolder", "question_image");

    for (var key of formData.entries()) {
      console.log(key[0] + ", " + key[1]);
    }

    axios
      .post("/api/admin/createimagequestion", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      })
      .then((r) => console.log(r))
      .catch((e) => console.log(e));
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    setImage(e.currentTarget.files?.item(0));
  };

  return (
    <div>
      <div>
        <form onSubmit={handleSubmit}>
          <input type={"file"} name={"image"} onChange={handleChange} />
          <input type={"text"} name={"question"} placeholder={"Вопрос"} />
          <input type={"text"} name={"option1"} placeholder={"option1"} />
          <input type={"text"} name={"option2"} placeholder={"option2"} />
          <input type={"text"} name={"hint"} placeholder={"hint"} />
          <input
            type={"text"}
            name={"correctAnswer"}
            placeholder={"correctAnswer"}
          />
          <input type={"submit"} />
        </form>
      </div>
    </div>
  );
};

export default Images;
